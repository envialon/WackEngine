#pragma once

#include "ComponentCommon.h"
#include "Transform.h"

namespace wack {


#define INIT_INFO(component) namespace component {struct init_info;}
	INIT_INFO(transform);
#undef INIT_INFO

	namespace game_entity {

		struct EntityInfo {
			transform::InitInfo* transform{ nullptr };
		};

		GameEntity create_game_entity(const EntityInfo& info);
		void remove_game_entity(GameEntity gameEntity);
		void remove_game_entity_by_id(EntityId id);
		bool is_valid(GameEntity gameEntity);
	}
}