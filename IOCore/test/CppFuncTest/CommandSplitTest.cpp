#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <iostream>
#include <vector>
using namespace std;

// 此代码仅能在PC上运行
vector<string> splitString(const char* str, const char* delimiter)
{
    vector<string> result;
    char* pc = strdup(str); 
    if (!pc) return result; 

    char* token = strtok(pc, delimiter);
    while (token != NULL)
    {
        result.push_back(string(token));
        token = strtok(NULL, delimiter);
    }

    free(pc);
    return result;
}

const char* getSingleParameter(const vector<string>& parts, int index)
{
    if (index >= 0 && index < parts.size())
    {
        return parts[index].c_str();
    }
    return NULL;
}

int main()
{
    char str[100];
    int ind;
    cout << "Input Command For Test: ";
    cin.getline(str, 100);
    cout << "Index To: ";
    cin >> ind;

    vector<string> parts = splitString(str, "#");
    const char* ptr = getSingleParameter(parts, ind);
    if (ptr)
    {
        cout << ptr << endl;
    }
    else
    {
        cout << "No parameter found at index " << ind << endl;
    }

    return 0;
}
