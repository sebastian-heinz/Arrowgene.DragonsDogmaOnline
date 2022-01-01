#include <windows.h>
#include <sstream>
#include <fstream>
#include <iostream>
#include <locale>
#include <codecvt>

#include "memory.h"
#include "util.h"
#include "ddo.h"

#include "ini.h"

void dummy() {
    MessageBox(NULL, L"dummy()", L"ddo.dll", NULL);
}

// hooking setup - start
constexpr int MAX_HOOKS = 10;

struct hook
{
    DWORD base_addr;     // base addr
    DWORD org_fn_rva;  // addrs of hooked original fn
    DWORD caller_fn_rva[MAX_HOOKS]; // where the hook should start
    LPVOID hook_fn_addr; // addr of function to call instead
    int steal_bytes_num;
};

void hook_fn(DWORD baseAddr, DWORD offset, LPVOID fnAddr) {
    DWORD patchHookAddr = baseAddr + offset;
    DWORD relativeFnHookAddr = (DWORD)((char*)fnAddr - (char*)(patchHookAddr + 1 + 4));
    const char* patchInitStart = "\xE8";
    WriteMemory((LPVOID)patchHookAddr, patchInitStart, 1);
    BYTE bRelativeHookInitAddr[4];
    memcpy(bRelativeHookInitAddr, &relativeFnHookAddr, 4);
    WriteMemory((LPVOID)(patchHookAddr + 1), bRelativeHookInitAddr, 4);
}

void execute_hook(hook h) {
    for (int i = 0; i < MAX_HOOKS; i++) {
        DWORD caller_fn_rva = h.caller_fn_rva[i];
        if (!caller_fn_rva) {
            return;
        }
        hook_fn(h.base_addr, caller_fn_rva, h.hook_fn_addr);
    }
}


/// <summary>
/// inserts a jmp anywhere by allocating memory close to the region to jump and restores stack + registers before jmp to original function.
/// </summary>
void jmp_detour(hook h) {

    uintptr_t pLoc = h.base_addr - (0x1000 * 5);
    void* trampLocation = nullptr;
    while (trampLocation == nullptr)
    {
        trampLocation = VirtualAlloc((void*)pLoc, 0x100, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);
        pLoc += 0x500;
    }
    DWORD patch_addr = (DWORD)trampLocation;
    DWORD abs_org_func_call_loc = (h.base_addr + h.org_fn_rva);

    // copy bytes overwritten by jmp
    byte* orginal_function_bytes = new byte[h.steal_bytes_num];
    ReadMemoryMem((LPVOID)abs_org_func_call_loc, orginal_function_bytes, h.steal_bytes_num);

    //insert jmp
    DWORD rel_patch = patch_addr - abs_org_func_call_loc - 5;
    WriteMemory((LPVOID)(abs_org_func_call_loc), "\xE9", 1);
    WriteMemory((LPVOID)(abs_org_func_call_loc + 1), &rel_patch, 4);

    int offset = 0;

    // esp
    //WriteMemory((LPVOID)(patch_addr + offset), "\x54", 1);
   // offset += 1;

    // remember registers & flags
   // WriteMemory((LPVOID)(patch_addr + offset), "\x50\x53\x51\x52\x56\x57\x9C", 7);
   // offset += 7;

    WriteMemory((LPVOID)(patch_addr + offset), "\x9C\x60", 2);
    offset += 2;


    WriteMemory((LPVOID)(patch_addr + offset), "\x51", 1);
    offset += 1;

    // call function
    WriteMemory((LPVOID)(patch_addr + offset), "\xE8", 1); 
    offset += 1;
    DWORD patchHookAddr = patch_addr + offset;
    DWORD relativeFnHookAddr = (DWORD)((char*)h.hook_fn_addr - (char*)(patchHookAddr  + 4));
    BYTE bRelativeHookInitAddr[4];
    memcpy(bRelativeHookInitAddr, &relativeFnHookAddr, 4);
    WriteMemory((LPVOID)(patch_addr + offset), bRelativeHookInitAddr, 4); // abs func address
    offset += 4;


    WriteMemory((LPVOID)(patch_addr + offset), "\x83\xC4\x04", 3);
    offset += 3;
    WriteMemory((LPVOID)(patch_addr + offset), "\x61\x9D", 2);
    offset += 2;

    //WriteMemory((LPVOID)(patch_addr + offset), "\xFF\xD6", 2); // call rsi
    //offset += 2;
    // restore stack pointer
    //WriteMemory((LPVOID)(patch_addr + offset), "\x49\x8B\xE4", 3); // mov r12, rsp
    //offset += 3;

    // restore registers & flags
   // WriteMemory((LPVOID)(patch_addr + offset), "\x9D\x5F\x5E\x5A\x59\x5B\x58", 7);
   // offset += 7;

    // restore esp
   // WriteMemory((LPVOID)(patch_addr + offset), "\x5C", 1);
   // offset += 1;

    // write overwritten insturctions
    WriteMemory((LPVOID)(patch_addr + offset), orginal_function_bytes, h.steal_bytes_num);
    offset += h.steal_bytes_num;

    // jmp back
    DWORD rel_loc = abs_org_func_call_loc - (patch_addr + offset); // +5 to skip original instruction
    WriteMemory((LPVOID)(patch_addr + offset), "\xE9", 1);
    offset += 1;
    // jmp addr 
    WriteMemory((LPVOID)(patch_addr + offset), &rel_loc, 4);
    offset += 4;
}

// hook setup - end

// global vars - start
hook h_bigint_sub;
hook h_bigint_add;

HMODULE base;
DWORD base_addr;

bool enabled;
bool enable_sub;
bool enable_add;

// global vars - end

// hooks - start
typedef int (*org_bigint_sub)(void* a, void* b, void* c, void* d);
typedef int (*org_bigint_add)(void* a);
typedef void (*jmp_hook)(void* eax, void* ebx, void* ecx, void* edx, void* esi, void* edi);

/// <summary>
/// Subtracts a - b, stores result in a
/// </summary>
int bigint_sub(void* a, void* b, void* c, void* d) {
    org_bigint_sub org = (org_bigint_sub)(h_bigint_sub.base_addr + h_bigint_sub.org_fn_rva);
    show((char*)a, 0x210, false);
    show((char*)b, 0x210, false);
    int r = org(a, b, c, d);
    show((char*)a, 0x210, false);
    show((char*)b, 0x210, false);
    return r;
}

int bigint_add(void* a) {
    org_bigint_add org = (org_bigint_add)(h_bigint_add.base_addr + h_bigint_add.org_fn_rva);
    show((char*)a, 0x210, false);
    int r = org(a);
    show((char*)a, 0x210, false);
    return r;
}

void jmp_bigint_add(void* eax, void* ebx, void* ecx, void* edx, void* esi, void* edi) {
   // show((char*)eax, 0x210, false);
}

// hooks - end

void setup() {
    h_bigint_sub = {
        base_addr,
        0xFEBC60,
        {0xFEB98C, 0xFEC600, 0xFEC9DF, 0xFECB79},
        bigint_sub,
    };
    if (enable_sub) {
     //   execute_hook(h_bigint_sub);
    }

    h_bigint_add = {
    base_addr,
    0xFEC0C0,
    {0xFEB8E4, 0xFEC592, 0xFEC5CF},
    jmp_bigint_add,
    6
    };
    if (enable_add) {
        jmp_detour(h_bigint_add);
     //   execute_hook(h_bigint_add);
    }


}

BOOL APIENTRY DllMain(HMODULE hModule,
    DWORD  ul_reason_for_call,
    LPVOID lpReserved
)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    {
        // find image base offsets
        base = GetModuleHandle(L"DDO_DUMP_FIX.exe");
        if (base == NULL) {
            fprintf(stderr, "Module [DDO_DUMP_FIX.exe] not found \n");
            return -1;
        }
        base_addr = (DWORD)base;
        fprintf(stdout, "base: %p \n", base);
        fprintf(stdout, "base_addr: %u \n", base_addr);

        // set default values
        enabled = false;
        enable_add = false;
        enable_sub = false;

        // load ini
        CSimpleIniA ini;
        ini.SetUnicode();
        if (ini.LoadFile("ddo.ini") < 0) {
            // create default ini
            ini.SetBoolValue("ddo", "enabled", enabled);
            ini.SetBoolValue("bigint", "add", enable_add);
            ini.SetBoolValue("bigint", "sub", enable_sub);
            if (ini.SaveFile("ddo.ini") < 0) {
                // ini create error
            }
        }

        enabled = ini.GetBoolValue("ddo", "enabled", enabled);
        enable_add = ini.GetBoolValue("bigint", "add", enable_add);
        enable_sub = ini.GetBoolValue("bigint", "sub", enable_sub);

        if (!enabled) {
            break;
        }

        // open console
        if (TRUE == AllocConsole())
        {
            FILE* nfp[3];
            freopen_s(nfp + 0, "CONOUT$", "rb", stdin);
            freopen_s(nfp + 1, "CONOUT$", "wb", stdout);
            freopen_s(nfp + 2, "CONOUT$", "wb", stderr);
            std::ios::sync_with_stdio();
        }

        // print current settings
        fprintf(stdout, "debug: %s \n", enabled ? "true" : "false");
        fprintf(stdout, "enable_add: %s \n", enable_add ? "true" : "false");
        fprintf(stdout, "enable_sub: %s \n", enable_sub ? "true" : "false");

        setup();
        break;
    }
    case DLL_THREAD_ATTACH:
        break;
    case DLL_THREAD_DETACH:
        break;
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

