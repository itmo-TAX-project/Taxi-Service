### Taxi-Service — мастер-система для водителей и автомобилей
#### Сущности
- Driver { driver_id, account_id, name, license_number, current_vehicle_id, allowed_segments, rating }
- Vehicle { vehicle_id, driver_id, segment (basic/mid/premium), plate, model, capacity }
- Snapshot: DriverStatus { driver_id, location, availability (searching/idle/busy/offline), ts }
#### gRPC
```
service TaxiService{
  rpc GetDriver(GetDriverRequest) returns (GetDriverResponse);
  rpc ValidateDriverActive(ValidateDriverRequest) returns (ValidateDriverResponse);
}
```
ValidateDriverActive — проверка активен ли водитель в данный момент
#### Kafka
* Publishes: taxi_driver_created, taxi_driver_status_changed, taxi_vehicle_changed.
* Subscribes: taxi_driver_create, taxi_driver_status_change, taxi_vehicle_change, taxi_vehicle_create, account_deleted.