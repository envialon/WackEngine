#pragma once

#include "ComponentCommon.h"
#include "Transform.h"

namespace wack {


#define INIT_INFO(component) namespace component {struct init_info;}
	INIT_INFO(transform);
#undef INIT_INFO

	namespace game_entity {

		struct entity_info {
			transform::init_info* transform{ nullptr };
		};

		GameEntity create_game_entity(const entity_info& info);
		void remove_game_entity(GameEntity gameEntity);
		bool is_valid(GameEntity gameEntity);
	}
}