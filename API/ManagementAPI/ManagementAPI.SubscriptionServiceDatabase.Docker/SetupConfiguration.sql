USE SubscriptionServiceConfiguration;

INSERT INTO `SubscriptionServices` (`SubscriptionServiceId`, `Description`) 
VALUES 
('f95c70a8-980f-4794-9576-49841b9d0993','Main Service');

INSERT INTO `EndPoints` (`EndPointId`, `Name`, `Url`) 
VALUES 
('3da27dd3-32cc-11e9-ab5f-00155d0d4221','Golf Club Membership Events','http://localhost:5000/api/DomainEvent/GolfClubMembership'),
('a05ee7ce-fd4f-11e8-ac9e-00155d0d422e','Golf Club Events','http://localhost:5000/api/DomainEvent/GolfClub');

INSERT INTO `SubscriptionStream` (`Id`, `StreamName`, `SubscriptionType`) 
VALUES 
('82bff2a8-32cc-11e9-ab5f-00155d0d4221','$ce-GolfClubAggregate',0),
('82bff2cf-32cc-11e9-ab5f-00155d0d4221','$ce-GolfClubMembershipAggregate',0);

INSERT INTO `SubscriptionGroups` (`Id`, `BufferSize`, `EndPointId`, `Name`, `StreamPosition`, `SubscriptionStreamId`) 
VALUES 
('bbf6b7e0-32e5-11e9-ab5f-00155d0d4221',10,'a05ee7ce-fd4f-11e8-ac9e-00155d0d422e','Golf Club Events',NULL,'82bff2a8-32cc-11e9-ab5f-00155d0d4221'),
('bbf6b80b-32e5-11e9-ab5f-00155d0d4221',10,'3da27dd3-32cc-11e9-ab5f-00155d0d4221','Golf Club Membership Events',NULL,'82bff2cf-32cc-11e9-ab5f-00155d0d4221');

INSERT INTO `SubscriptionServiceGroups` (`SubscriptionServiceGroupId`, `SubscriptionGroupId`, `SubscriptionServiceId`) 
VALUES 
('f765b596-32e5-11e9-ab5f-00155d0d4221','bbf6b7e0-32e5-11e9-ab5f-00155d0d4221','f95c70a8-980f-4794-9576-49841b9d0993'),
('f765b5ba-32e5-11e9-ab5f-00155d0d4221','bbf6b80b-32e5-11e9-ab5f-00155d0d4221','f95c70a8-980f-4794-9576-49841b9d0993');


