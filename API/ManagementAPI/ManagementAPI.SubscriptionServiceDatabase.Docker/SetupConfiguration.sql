USE SubscriptionServiceConfiguration;

INSERT INTO `SubscriptionServices` (`SubscriptionServiceId`, `Description`) 
VALUES 
('f95c70a8-980f-4794-9576-49841b9d0993','Main Service');

INSERT INTO `EndPoints` (`EndPointId`, `Name`, `Url`) 
VALUES 
('3da27dd3-32cc-11e9-ab5f-00155d0d4221','Golf Club Membership Events','http://4266ba38.ngrok.io/api/DomainEvent/GolfClubMembership'),
('a05ee7ce-fd4f-11e8-ac9e-00155d0d422e','Golf Club Events','http://4266ba38.ngrok.io/api/DomainEvent/GolfClub'),
('e88f44b5-c4e7-4709-a9bf-f2c0f8225638', 'Tournament Events', 'http://4266ba38.ngrok.io/api/DomainEvent/Tournament'),
('e2eb435b-fdb4-4de4-bc5b-cde1dec06bab', 'Handicap Calculator Process Events', 'http://4266ba38.ngrok.io/api/DomainEvent/HandicapCalculator');

INSERT INTO `SubscriptionStream` (`Id`, `StreamName`, `SubscriptionType`) 
VALUES 
('82bff2a8-32cc-11e9-ab5f-00155d0d4221','$ce-GolfClubAggregate',0),
('82bff2cf-32cc-11e9-ab5f-00155d0d4221','$ce-GolfClubMembershipAggregate',0),
('a1e440c1-f307-4c8e-ae21-695b4e919868', '$ce-TournamentAggregate', 0),
('46267276-8f71-4557-9d5d-b74e9ad62b45', '$ce-HandicapCalculationProcessAggregate', 0);

INSERT INTO `SubscriptionGroups` (`Id`, `BufferSize`, `EndPointId`, `Name`, `StreamPosition`, `SubscriptionStreamId`) 
VALUES 
('bbf6b7e0-32e5-11e9-ab5f-00155d0d4221',10,'a05ee7ce-fd4f-11e8-ac9e-00155d0d422e','Golf Club Events',NULL,'82bff2a8-32cc-11e9-ab5f-00155d0d4221'),
('bbf6b80b-32e5-11e9-ab5f-00155d0d4221',10,'3da27dd3-32cc-11e9-ab5f-00155d0d4221','Golf Club Membership Events',NULL,'82bff2cf-32cc-11e9-ab5f-00155d0d4221'),
('3de47a15-7b5c-4e34-972d-190af8c9be80', 10, 'e88f44b5-c4e7-4709-a9bf-f2c0f8225638', 'Tournament Events', null, 'a1e440c1-f307-4c8e-ae21-695b4e919868'),
('efd5020c-9fa1-4e82-880c-2e68aa053bbf', 10, 'e2eb435b-fdb4-4de4-bc5b-cde1dec06bab', 'Handicap Calculator Process Events', null, '46267276-8f71-4557-9d5d-b74e9ad62b45');

INSERT INTO `SubscriptionServiceGroups` (`SubscriptionServiceGroupId`, `SubscriptionGroupId`, `SubscriptionServiceId`) 
VALUES 
('f765b596-32e5-11e9-ab5f-00155d0d4221','bbf6b7e0-32e5-11e9-ab5f-00155d0d4221','f95c70a8-980f-4794-9576-49841b9d0993'),
('f765b5ba-32e5-11e9-ab5f-00155d0d4221','bbf6b80b-32e5-11e9-ab5f-00155d0d4221','f95c70a8-980f-4794-9576-49841b9d0993'),
('9539605d-750c-4018-9963-926a8f54c9e0', '3de47a15-7b5c-4e34-972d-190af8c9be80', 'f95c70a8-980f-4794-9576-49841b9d0993'),
('2ebfc970-9491-4634-aa94-45e49a55f80e', 'efd5020c-9fa1-4e82-880c-2e68aa053bbf', 'f95c70a8-980f-4794-9576-49841b9d0993');


