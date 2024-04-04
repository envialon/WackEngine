#pragma once

#include "..\Components\ComponentCommon.h"
#include "TransformComponent.h"

namespace wack::game_entity {

	DEFINE_TYPED_ID(EntityId);


	class GameEntity {
	private:
		EntityId _id;


	public:
		constexpr explicit GameEntity(EntityId id) : _id{ id } {}
		constexpr GameEntity() : _id{ id::invalid_id } {}
		constexpr EntityId get_id() const { return _id; }
		constexpr bool is_valid() const { return id::is_valid(_id); }

		transform::TransformComponent transform() const;
	};
}