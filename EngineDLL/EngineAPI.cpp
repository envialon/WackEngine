/// To avoid the c++ compiler changing the name of the function
/// see name mangling
#ifndef EDITOR_INTERFACE
#define EDITOR_INTERFACE extern "C" __declspec(dllexport)
#endif //!EDITOR_INTERFACE

#include "CommonHeaders.h"
#include "Id.h"
#include "..\Engine\Components\Entity.h"
#include "..\Engine\Components\Transform.h"

using namespace wack;

namespace {
	struct TransformDescriptor {
		f32 position[3];
		f32 rotation[3];
		f32 scale[3];

		transform::InitInfo to_init_info() {
			using namespace DirectX;
			transform::InitInfo info{};
			memcpy(&info.position[0], &position[0], sizeof(f32) * _countof(position));
			memcpy(&info.scale[0], &scale[0], sizeof(f32) * _countof(scale));
			XMFLOAT3A rot{ &rotation[0] };
			XMVECTOR quat{ XMQuaternionRotationRollPitchYawFromVector(XMLoadFloat3A(&rot)) };
			XMFLOAT4A rot_quat{};
			XMStoreFloat4A(&rot_quat, quat);
			memcpy(&info.rotation[0], &rot_quat.x, sizeof(f32) * _countof(info.rotation));
			return info;
		}
	};

	struct GameEntityDescriptor {
		TransformDescriptor transform;
	
	};
}

EDITOR_INTERFACE id::id_type
CreateGameEntity(GameEntityDescriptor* entityDescriptor) {
	assert(entityDescriptor);
	GameEntityDescriptor& desc{ *entityDescriptor };
	
	transform::InitInfo transformInfo{ desc.transform.to_init_info() };
	game_entity::EntityInfo entityInfo{
		&transformInfo,
	};

	return game_entity::create_game_entity(entityInfo).get_id();
}

EDITOR_INTERFACE void
RemoveGameEntity(id::id_type id) {
	assert(id::is_valid(id));
	game_entity::remove_game_entity_by_id(game_entity::EntityId(id));
}