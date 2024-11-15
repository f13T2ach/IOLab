/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * WorkshopPage.xaml.cs is part of the IOLab project.
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

using IOLab.Core.Network.Data;
using IOLab.Resources.Control;
using Newtonsoft.Json.Linq;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace IOLab.Pages
{
    public partial class WorkshopPage : Page
    {
        public ContentCollection WorkshopContents { get; set; }

        public WorkshopPage()
        {
            InitializeComponent();

            WorkshopContents = new ContentCollection();

            // 示例数据
            var networkData = JObject.Parse(@"{
            'items': [
                { 'title': '示例标题1', 'author': '作者1', 'content': '当天，中国驻菲律宾大使黄溪连就菲方签署“海洋区域法”和“群岛海道法”向菲外交部负责人提出严正交涉，并就中方划定并公布黄岩岛领海基点基线表明立场，强调这是对菲出台“海洋区域法”的必要回应，是中国政府依法加强海洋管理的正常举措，符合国际法和国际惯例。中方将继续依法采取必要措施，维护自身领土主权和海洋权益。“我们敦促菲方立即停止采取可能导致争议扩大化、局势复杂化的单方面举动，维护南海和平稳定。”', 'created_at': '2024-11-14T08:00:00' },
                { 'title': '示例标题2', 'author': '作者2', 'content': '教育部部署各地各高校实施“2025届全国普通高校毕业生就业创业促进和服务体系建设行动”，多措并举促进高校毕业生就业。据介绍，9月以来，各地各高校抢抓秋招关键期，不断拓展岗位资源，截至目前各地各高校已开展招聘活动5.5万场，提供岗位信息1093万条。', 'created_at': '2024-11-14T09:00:00' }
            ]
        }");

            WorkshopContents.Parse(networkData);

            // 设置 DataContext 以便 XAML 绑定
            DataContext = this;
        }
    }

}
