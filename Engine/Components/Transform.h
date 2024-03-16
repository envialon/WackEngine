#pragma once

#include "ComponentCommon.h"
#include "../EngineAPI/TransformComponent.h"

namespace wack::transform {
	struct InitInfo {
		f32 position[3]{};
		f32 rotation[4]{};
		f32 scale[3]{ 1.f, 1.f, 1.f };
	};

	TransformComponent create_transform(const InitInfo& info, game_entity::GameEntity entity);
	void remove_transform(TransformComponent c);
}