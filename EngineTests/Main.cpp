#pragma comment(lib, "Engine.lib")


#define TEST_ENTITY_COMPONENTS 1

#if TEST_ENTITY_COMPONENTS
#include "./TestInclude/ECS_Test.h"
#else
#error One of the tests need to be enabled
#endif
int main() {

#if _DEBUG
	_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
#endif

	entity_test test = entity_test();

	if (test.initialize()) test.run();
	test.shutdown();


}