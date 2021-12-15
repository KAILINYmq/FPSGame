package main


import (
	"github.com/xiaonanln/goworld/engine/entity"
)

type MySpace struct {
	entity.Space // Space type should always inherit from entity.Space
}

// OnSpaceCreated is called when the space is created
func (space *MySpace) OnSpaceCreated() {

}