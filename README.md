### 3. Taxi-Service — мастер-система для водителей и автомобилей
#### Сущности
- Driver { driver_id, account_id, name, license_number, current_vehicle_id, allowed_segments, rating }
- Vehicle { vehicle_id, driver_id, segment (basic/mid/premium), plate, model, capacity }
- Snapshot: DriverStatus { driver_id, location, availability (searching/idle/busy), ts } (публикуется в Kafka, сказать на защите)
#### gRPC
```
service TaxiMaster {
  rpc CreateDriver(CreateDriverRequest) returns (CreateDriverResponse);
  rpc GetDriver(GetDriverRequest) returns (GetDriverResponse);
  rpc UpdateVehicle(UpdateVehicleRequest) returns (UpdateVehicleResponse);
  rpc ValidateDriverActive(ValidateDriverRequest) returns (ValidateDriverResponse);
}
```
ValidateDriverActive — проверка активен ли водитель в данный момент
#### Kafka
Publishes: taxi.driver.created, taxi.driver.status_changed, taxi.vehicle.changed.
Subscribes: account.deleted.