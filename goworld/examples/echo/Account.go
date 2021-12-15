package main

import (
	"github.com/xiaonanln/goworld/engine/entity"
)

// 玩家类型
type Account struct {
	// 自定义对象类型必须继承entity.Entity
	entity.Entity
}

// OnCreated 在Player对象创建后被调用
func (a *Account) OnCreated() {
}


func (a *Account) Echo_Client(msg string) {
	a.CallClient("ShowInfo", msg)
}

func (a *Account) DescribeEntityType(desc *entity.EntityTypeDesc) {

}