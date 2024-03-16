#pragma once

#include "..\Components\ComponentCommon.h"

namespace wack::transform {

	DEFINE_TYPED_ID(transform_id);


	class TransformComponent final {
	private:
		transform_id _id;

	public:

		constexpr explicit TransformComponent(transform_id id) : _id{ id } {}
		constexpr explicit TransformComponent() : _id{ id::invalid_id } {}
		constexpr transform_id get_id() const { return _id; };
		constexpr bool is_valid() const { return id::is_valid(_id); }
	
		math::v4 rotation() const;
		math::v3 position() const;
		math::v3 scale() const;
	};

}