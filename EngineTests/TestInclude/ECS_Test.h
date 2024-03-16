#pragma once

#include ".\Test.h";
#include "..\Components\Entity.h"
#include "..\Components\Transform.h"

#include <iostream>
#include <ctime>

using namespace wack; //this is specifically to test the engine that's why this is in a .h

class entity_test : public Test {
private:
	utl::vector<game_entity::GameEntity> entities;
	u32 added{ 0 };
	u32 removed{ 0 };
	u32 num_entities{ 0 };

	void create_random() {
		u32 count = rand() % 20;
		if (entities.empty()) count = 1000;

		transform::InitInfo transform_info{} ;
		game_entity::EntityInfo entity_info{
			&transform_info,
		};

		for (int i = 0; i < count;i++) {
			added++;
			game_entity::GameEntity entity{ game_entity::create_game_entity(entity_info) };
			assert(entity.is_valid()); //&& id::is_valid(entity.get_id()));
			entities.push_back(entity);
		}
	}
	
	void remove_random() {
		u32 count = rand() % 20;
		if (entities.size() < 1000) return;
		
		for (int i = 0; i < count; i++) {
			const u32 index = (u32)rand() % entities.size();
			const game_entity::GameEntity entity{ entities[index] };

			assert(entity.is_valid());

			game_entity::remove_game_entity(entity);
			entities.erase(entities.begin() + index);
			removed++; 

			assert(!game_entity::is_valid(entity));
		}
	}

	void print_results() {
		std::cout << "Entities created: " << added << "\n";
		std::cout << "Entities deleted: " << removed << "\n";

	}

public:
	bool initialize() override {
		srand((u32)time(nullptr));
		return true;
	}

	void run() override {
		do {
			for (u32 i = 0; i < 10000; i++) {
				create_random();
				remove_random();
				num_entities = (u32)entities.size();
			}
		} while (getchar() != 'q');
		print_results();
	}

	void shutdown() override {
	}
};