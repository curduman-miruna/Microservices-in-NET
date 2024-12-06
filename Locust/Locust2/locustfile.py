from locust import HttpUser, task, between

class DemoUser(HttpUser):
    wait_time = between(1, 3)

    @task
    def create_and_delete_item(self):
        response = self.client.post("/INVENTORY-SERVICE/api/inventory", json={
            "Product": "Carrot",
            "Quantity": 100
        })
        if response.status_code == 201:
            item_id = response.json().get("id")
            if item_id:
                self.client.delete(f"/INVENTORY-SERVICE/api/inventory/{item_id}")

    @task
    def create_and_delete_order(self):
        response = self.client.post("/ORDER-SERVICE/api/orders", json={
            "Product": "Carrot",
            "Quantity": 50,
            "Price": 25.5
        })
        if response.status_code == 201:
            order_id = response.json().get("id")
            if order_id:
                self.client.delete(f"/ORDER-SERVICE/api/orders/{order_id}")

    @task
    def view_items(self):
        response = self.client.get("/INVENTORY-SERVICE/api/inventory")
        if response.status_code == 200 and response.json():
            item_id = response.json()[0].get("id")
            if item_id:
                self.client.get(f"/INVENTORY-SERVICE/api/inventory/{item_id}")

    @task
    def view_orders(self):
        response = self.client.get("/ORDER-SERVICE/api/orders")
        if response.status_code == 200 and response.json():
            order_id = response.json()[0].get("id")
            if order_id:
                self.client.get(f"/ORDER-SERVICE/api/orders/{order_id}")