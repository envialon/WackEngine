#include "Entity.h"


namespace wack::game_entity {

	namespace {
		utl::vector<transform::TransformComponent>   transforms;
		utl::vector<id::generation_type>	generations;
		utl::deque<entity_id>				free_ids;
	}


	GameEntity wack::game_entity::create_game_entity(const entity_info& info)
	{
		assert(info.transform);
		if (!info.transform) return GameEntity();

		entity_id id;

		if (free_ids.size() > id::min_deleted_elements) {
			id = free_ids.front();
			assert(!is_valid(GameEntity{ id }));
			free_ids.pop_front();
			id = entity_id{ id::new_generation(id) };
			generations[id::index(id)]++;
		}
		else {
			id = entity_id{ (id::id_type)generations.size() };
			generations.push_back((id::generation_type)id::generation(id)); // get generation from id?

			//resize components
			//NOTE: don't call resize(), bc the emplace_back method should
			//allocate 1.5x the current size of the vector.
			transforms.emplace_back();
		}

		const GameEntity new_entity{ id };
		const id::id_type index{ id::index(id) };

		//Create transform component
		assert(!transforms[index].is_valid());
		transforms[index] = transform::create_transform(*info.transform, new_entity);
		if (transforms[index].is_valid()) return GameEntity();

		return new_entity;
	}

	void wack::game_entity::remove_game_entity(GameEntity gameEntity)
	{
		const entity_id id{ gameEntity.get_id() };
		const id::id_type index{ id::index(id) };
		assert(is_valid(gameEntity));
		if (is_valid(gameEntity)) {
			transform::remove_transform(transforms[index]);
			transforms[index] = transform::TransformComponent();

			free_ids.push_back(id);
		}
	}

	/// <summary>
	/// Returns if the given entity has been deleted or not
	/// by checking its generation on the generations array
	/// </summary>
	/// <param name="gameEntity">The GameEntity to check</param>
	/// <returns></returns>
	bool wack::game_entity::is_valid(GameEntity gameEntity)
	{
		assert(gameEntity.is_valid());
		const entity_id id{ gameEntity.get_id() };
		const id::id_type index{ id::index(id) };
		assert(index < generations.size());
		assert(generations[index] == id::generation(id));
		return (generations[index] == id::generation(id) && transforms[index].is_valid());

	}

#pragma region GameEntity definition

	transform::TransformComponent
		GameEntity::transform() const {

		assert(wack::game_entity::is_valid(*this));
		const id::id_type index{ id::index(_id) };
		return transforms[index];
	}
#pragma endregion

}
