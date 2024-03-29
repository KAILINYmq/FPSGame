package main

import (
	"github.com/xiaonanln/goworld"
)

func main() {
	// 注册自定义的Space类型（必须提供）
	goworld.RegisterSpace(&MySpace{})
	// 注册Account类型
	goworld.RegisterEntity("Account", &Account{})
	// 运行游戏服务器
	goworld.Run()
}