#pragma once



template <typename I> std::string to_hex(I* bytes, int size, bool stop_at_null) {
	static const char hex[16] = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B','C','D','E','F' };
	std::string str;
	for (int i = 0; i < size; ++i) {
		const char ch = bytes[i];
		if (stop_at_null && ch == 0) {
			break;
		}
		str.append(&hex[(ch & 0xF0) >> 4], 1);
		str.append(&hex[ch & 0xF], 1);
		str.append("-");
	}
	return str;
}

template <typename I> std::string to_ascii(I* bytes, int size, bool stop_at_null) {
	std::string str;
	for (int i = 0; i < size; ++i) {
		const char ch = bytes[i];
		if (ch >= 32 && ch <= 127) {
			str.append(&ch, 1);
		}
		else {
			if (stop_at_null && ch == 0) {
				break;
			}
			str.append(".");
		}
	}
	return str;
}

template <typename I> std::string read_msg(I* bytes, int len) {
	std::string str;
	int idx = 0;
	while (idx < len) {
		const char ch = bytes[idx];
		str.append(&ch, 1);
		idx+= 2;
	}
	return str;
}

template <typename I>void show(I* bytes, int size, bool stop_at_null) {
	fprintf(stdout, "\n");
	fprintf(stdout, "---------\n");
	fprintf(stdout, "Size: %d\n", size);
	fprintf(stdout, "%s\n", to_ascii(bytes, size, stop_at_null).c_str());
	fprintf(stdout, "%s\n", to_hex(bytes, size, stop_at_null).c_str());
	fprintf(stdout, "---------\n");
	fprintf(stdout, "\n");
}

void write_file(char const* filename, BYTE* fileData, DWORD fileLen)
{
	std::ofstream ofile(filename, std::ios::binary);
	ofile.write((char*)fileData, fileLen);
}

inline bool file_exists(std::wstring file_name) {
	struct _stat file;
	return _wstat(file_name.c_str(), &file) == 0;
}

inline bool file_exists(std::string file_name) {
	struct _stat file;
	return _stat(file_name.c_str(), &file) == 0;
}

std::wstring s_2_ws(const std::string& str)
{
	using convert_typeX = std::codecvt_utf8<wchar_t>;
	std::wstring_convert<convert_typeX, wchar_t> converterX;

	return converterX.from_bytes(str);
}

std::string ws_2_s(const std::wstring& wstr)
{
	using convert_typeX = std::codecvt_utf8<wchar_t>;
	std::wstring_convert<convert_typeX, wchar_t> converterX;

	return converterX.to_bytes(wstr);
}