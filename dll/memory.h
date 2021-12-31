#pragma once

template <typename I> void ReadMemory(LPVOID address, I& value, int byteNum)
{
	unsigned long OldProtection;
	VirtualProtect(address, byteNum, PAGE_EXECUTE_READWRITE, &OldProtection);
	value = *(I*)address;
	VirtualProtect(address, byteNum, OldProtection, &OldProtection);
}

template <typename I> void ChangeMemory(LPVOID address, I value, int byteNum)
{
	unsigned long OldProtection;
	VirtualProtect(address, byteNum, PAGE_EXECUTE_READWRITE, &OldProtection);
	*(I*)address = value;
	VirtualProtect(address, byteNum, OldProtection, &OldProtection);
}

template <typename I> void WriteMemory(LPVOID address, I value, int byteNum)
{
	unsigned long OldProtection;
	VirtualProtect(address, byteNum, PAGE_EXECUTE_READWRITE, &OldProtection);
	memcpy(address, value, byteNum);
	VirtualProtect(address, byteNum, OldProtection, &OldProtection);
}