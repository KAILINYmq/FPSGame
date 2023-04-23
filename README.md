# FPSGame

- 通过Unity3D开发了TPS游戏，TPS游戏分为客户端和服务器两部分，客户端实现从UI登录界面到游戏大厅的创建，以及多名玩家同时进入到一个房间开始同一局游戏，游戏所有的连接方式均为TCP连接，一些玩家通过UI做出的操作均使用RPC调用服务端(比如登录注册)。服务器部分使用Golang语言进行开发，主要包含了连接的管理，以及对客户端发送过来的数据包进行处理比如是否进行转发操作，游戏逻辑的处理主要包含了玩家的登录注册请求和一些攻击伤害的服务端处理，数据持久化使用MongoDB数据库。

- 调试运行
![image](https://user-images.githubusercontent.com/50624154/233821785-b2e06ff4-6518-494a-8ac6-ab4575748716.png)
![image](https://user-images.githubusercontent.com/50624154/233821795-2225e651-0691-41d1-a8a7-f6829203ef34.png)
![image](https://user-images.githubusercontent.com/50624154/233821633-f2ff46d5-0b3f-47c9-88e9-97682eb0be8c.png)

- 游戏流程

![image](https://user-images.githubusercontent.com/50624154/233821957-f9cfe8e4-123b-424a-a777-c9f9f472ce4d.png)

- c/s实现功能

![image](https://user-images.githubusercontent.com/50624154/233821943-52e70d07-d97b-4ba3-ac40-da32c8f20629.png)

## Client unity3D(>=2020版本)
## Server goworld
```
go install
goworld build examples/unity_demo
goworld start examples/unity_demo 
goworld status examples/unity_demo 
goworld stop examples/unity_demo
```

后端基于框架goworld开发：https://github.com/xiaonanln/goworld
