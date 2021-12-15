package main

import (
	"github.com/xiaonanln/goworld"
	"github.com/xiaonanln/goworld/engine/common"
	"github.com/xiaonanln/goworld/engine/entity"
	"github.com/xiaonanln/goworld/engine/gwlog"
)

// SpaceService is the service entity for space management
type SpaceService struct {
	entity.Entity
	gameSpaceID  common.EntityID
	loveSpaceID  common.EntityID
	sportSpaceID  common.EntityID
}

func (s *SpaceService) DescribeEntityType(desc *entity.EntityTypeDesc) {
}

// OnCreated is called when entity is created
func (s *SpaceService) OnCreated() {
	gwlog.Infof("Registering SpaceService ...")
	s.gameSpaceID = goworld.CreateSpaceAnywhere(1)
	s.loveSpaceID = goworld.CreateSpaceAnywhere(1)
	s.sportSpaceID = goworld.CreateSpaceAnywhere(1)
}


// 获取场景ID
func (s *SpaceService) GetSpaceID(callerID common.EntityID, channel string) {
	var id common.EntityID
	if channel == "0" {
		id = s.gameSpaceID
	} else if channel == "1" {
		id = s.loveSpaceID
	} else if channel == "2" {
		id = s.sportSpaceID
	} else {
		gwlog.Infof("channel err" )
	}

	s.Call(callerID, "OnGetSpaceID", id)
}

func  (s *SpaceService) OnEntityEnterSpace() {
}