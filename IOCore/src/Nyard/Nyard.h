/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * Nyard.h is part of the IOLab project.
 *
 * IOLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * IOLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with IOLab. If not, see <https://www.gnu.org/licenses/>.
 */

#ifndef NYARD_H
#define NYARD_H
#include<Arduino.h>
class Nyard {
public:
    // 构造函数
    Nyard(int id);
    
    // 虚函数（可被子类重写）
    virtual void setupPins() = 0;
    virtual void begin() = 0;
    virtual int collectData() = 0;
    virtual void setPins(const std::vector<int>& pins) = 0;

    // 获取id
    int getId() const {
        return id;
    }

protected:
    int id;
};

#endif

