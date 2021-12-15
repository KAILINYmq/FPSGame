package main

import (
	"github.com/xiaonanln/goworld"
	"github.com/xiaonanln/goworld/engine/entity"
	"github.com/xiaonanln/goworld/engine/common"
)

// 玩家类型
type Account struct {
	// 自定义对象类型必须继承entity.Entity
	entity.Entity
}

// OnCreated 在Player对象创建后被调用
func (a *Account) OnCreated() {

}

// OnGetSpaceID is called by SpaceService
func (a *Account) OnGetSpaceID(spaceID common.EntityID) {
	// let account enter space with spaceID
}

func (a *Account) Echo_Client(msg string) {
	a.Space.ForEachEntity(  func(e *entity.Entity) {
		e.CallClient("ShowInfo", msg)
	})

}

func (a *Account) Join_Client(channel string) {
	//goworld.CallService("SpaceService", "GetSpaceID", a.ID, channel)
	goworld.CallServiceAny("SpaceService", "GetSpaceID", a.ID, channel)
}

func (a *Account) DescribeEntityType(desc *entity.EntityTypeDesc) {

}