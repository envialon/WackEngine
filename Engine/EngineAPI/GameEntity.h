#pragma once

#include "../Components/ComponentCommon.h"
#include "TransformComponent.h"

namespace wack::game_entity {

	DEFINE_TYPED_ID(entity_id);


	class GameEntity {
	private:
		entity_id _id;


	public:
		constexpr explicit GameEntity(entity_id id) : _id{ id } {}
		constexpr GameEntity() : _id{ id::invalid_id } {}
		constexpr entity_id get_id() const { return _id; };
		constexpr bool is_valid() const { return id::is_valid(_id); }

		transform::TransformComponent transform() const;
	};
}