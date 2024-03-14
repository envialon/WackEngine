#pragma once

#include "..\Components\ComponentCommon.h"

namespace wack::transform {

	DEFINE_TYPED_ID(transform_id);


	class Component final {
	private:
		transform_id _id;

	public:

		constexpr explicit Component(transform_id id) : _id{ id } {}
		constexpr explicit Component() : _id{ id::invalid_id } {}
		constexpr transform_id get_id() const { return _id; };
		constexpr bool is_valid() const { return id::is_valid(_id); }
	};

}