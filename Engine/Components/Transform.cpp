#include "Transform.h"
#include "Entity.h"

namespace wack::transform {
	namespace {
		utl::vector<math::v4>	rotations;
		utl::vector<math::v3>	positions;
		utl::vector<math::v3>	   scales;		
	} 

	TransformComponent create_transform(const InitInfo& info, game_entity::GameEntity entity) {
		assert(entity.is_valid());
		const id::id_type entity_index{ id::index(entity.get_id()) };
		if (positions.size() > entity_index) {
			rotations[entity_index] = math::v4(info.rotation);
			positions[entity_index] = math::v3(info.position);
			scales[entity_index] = math::v3(info.scale);
		}
		else {
			assert(positions.size() == entity_index);
			rotations.emplace_back(info.rotation);
			positions.emplace_back(info.position);
			scales.emplace_back(info.scale);
		}
		return TransformComponent(transform_id{ entity.get_id()});
	}

	void remove_transform(TransformComponent c) {
		assert(c.is_valid());
	}

#pragma region TransformComponent definitions
	math::v4 TransformComponent::rotation() const {
		assert(is_valid());
		return rotations[id::index(_id)];
	}

	math::v3 TransformComponent::position() const {
		assert(is_valid()); 
		return positions[id::index(_id)];
	}

	math::v3 TransformComponent::scale() const {
		assert(is_valid()); 
		return scales[id::index(_id)];
	}
#pragma endregion


}