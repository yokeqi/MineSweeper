# 扫雷

#### 介绍
扫雷是windows系统自带的非常经典的一款小游戏。


#### 软件架构
Web应用：Html+JQuery+CSS


#### 设计思路

1. 创建数据地图
* 一个N*N的数组
* 坐标(i,j)代表一个格子
* 格子的值：0表示空白，大于100表示雷，0~100之间表示格子相邻有多少颗雷
2. 根据数据地图初始化UI
3. 挖雷、插旗
* 插旗就是一个Flag，用来快速校验通关用的而已
* 挖雷遵循1中的逻辑，其中如果挖到空白，则自动扩散将所有相邻空白都自动挖开直到遇到数字
4. 校验是否通关

#### 参与贡献

1.  Fork 本仓库
2.  新建 Feat_xxx 分支
3.  提交代码
4.  新建 Pull Request


#### 码云特技

1.  使用 Readme\_XXX.md 来支持不同的语言，例如 Readme\_en.md, Readme\_zh.md
2.  码云官方博客 [blog.gitee.com](https://blog.gitee.com)
3.  你可以 [https://gitee.com/explore](https://gitee.com/explore) 这个地址来了解码云上的优秀开源项目
4.  [GVP](https://gitee.com/gvp) 全称是码云最有价值开源项目，是码云综合评定出的优秀开源项目
5.  码云官方提供的使用手册 [https://gitee.com/help](https://gitee.com/help)
6.  码云封面人物是一档用来展示码云会员风采的栏目 [https://gitee.com/gitee-stars/](https://gitee.com/gitee-stars/)
