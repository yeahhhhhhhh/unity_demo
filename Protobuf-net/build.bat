protogen.exe --proto_path=. --csharp_out=../Assets/ProtobufClass attributes/combat.proto
protogen.exe --proto_path=. --csharp_out=../Assets/ProtobufClass attributes/common.proto
protogen.exe --proto_path=. --csharp_out=../Assets/ProtobufClass attributes/player.proto
protogen.exe --proto_path=. --csharp_out=../Assets/ProtobufClass attributes/scene.proto

protogen.exe --proto_path=. --csharp_out=../Assets/ProtobufClass service/account_service.proto
protogen.exe --proto_path=. --csharp_out=../Assets/ProtobufClass service/common_service.proto
protogen.exe --proto_path=. --csharp_out=../Assets/ProtobufClass service/player_service.proto
protogen.exe --proto_path=. --csharp_out=../Assets/ProtobufClass service/scene_service.proto

pause