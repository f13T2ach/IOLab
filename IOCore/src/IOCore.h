/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * IOCore.h is part of the IOLab project.
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

#include <Arduino.h>
#include "Nyard/Sample.h"
int coreVersion = 1;
String Authentication = "{\"command\":\"ShakeHand\",\"version\":1}"; // 认证消息
String ShankeHanded = "{\"command\":\"ShakeHand\",\"name\":\"瑞克五代\",\"code\":0}";
String CallbackNewPortSuccess = "{\"command\":\"NewPortCallback\",\"code\":0,\"msg\":\"委任端口成功\"}";
String CallbackNewPortFailed_Invalid1 = "{\"command\":\"NewPortCallback\",\"code\":1,\"msg\":\"委任的端口无效\"}";