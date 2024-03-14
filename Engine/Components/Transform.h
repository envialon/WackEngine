#pragma once

#include "ComponentCommon.h"
#include "../EngineAPI/TransformComponent.h"

namespace wack::transform {
	struct init_info {
		f32 position[3]{};
		f32 rotation[4]{};
		f32 scale[3]{ 1.f, 1.f, 1.f };
	};

	Component create_transform(const init_info& info, game_entity::GameEntity entity_id);
	void remove_transform(Component c);
}