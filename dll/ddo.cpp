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
// hook setup - end

// global vars - start
hook h_bigint_sub;
hook h_bigint_add;

HMODULE base;
DWORD base_addr;

bool enabled;

// global vars - end

// hooks - start
typedef int (*org_bigint_sub)(void* a, void* b, void* c, void* d);
typedef int (*org_bigint_add)(void* a);

/// <summary>
/// Subtracts a - b, stores result in a
/// </summary>
int bigint_sub(void* a, void* b, void* c, void* d) {
    org_bigint_sub org = (org_bigint_sub)(h_bigint_sub.base_addr + h_bigint_sub.org_fn_rva);
    show((char*)a, 0x210, false);
    show((char*)b, 0x210, false);
    int r = org(a,b,c,d);
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

// hooks - end

void setup() {
    h_bigint_sub = {
        base_addr,
        0xFEBC60,
        {0xFEB98C, 0xFEC600, 0xFEC9DF, 0xFECB79},
        bigint_sub,
    };
    execute_hook(h_bigint_sub);

    h_bigint_add = {
    base_addr,
    0xFEC0C0,
    {0x13EB8E4, 0x13EC5CF, 0x13EC592},
    bigint_add,
    };
    execute_hook(h_bigint_add);

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

        // load ini
        CSimpleIniA ini;
        ini.SetUnicode();
        if (ini.LoadFile("ddo.ini") < 0) {
            // create default ini
            ini.SetBoolValue("ddo", "enabled", enabled);
            //	ini.SetBoolValue("ddo", "recv_area_op_code", recv_area_op_code);
            //	ini.SetBoolValue("ddo", "use_item_decrypted", use_item_decrypted);
            if (ini.SaveFile("ddo.ini") < 0) {
                // ini create error
            }
        }

        enabled = ini.GetBoolValue("ddo", "enabled", enabled);

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

