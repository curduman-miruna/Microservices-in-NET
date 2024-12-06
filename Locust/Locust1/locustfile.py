from locust import HttpUser, task, between

class DemoUser(HttpUser):
    wait_time = between(1, 3)

    @task
    def create_and_delete_item(self):
        response = self.client.post("/inventoryservice/api/inventory", json={
            "Product": "Carrot-P",
            "Quantity": 100
        })
        if response.status_code == 201:
            print("Created item:", response.json())
            item_id = response.json().get("id")
            if item_id:
                delete_response = self.client.delete(f"/inventoryservice/api/inventory/{item_id}")
                print(f"Attempted to delete item {item_id}. Status: {delete_response.status_code}")
            else:
                print("Item creation response did not contain an ID.")
        else:
            print(f"Failed to create item. Status: {response.status_code}")

    @task
    def create_and_delete_order(self):
        response = self.client.post("/orderservice/api/order", json={
            "Product": "Carrot-P",
            "Quantity": 50,
            "Price": 25.5
        })
        if response.status_code == 201:
            order_id = response.json().get("id")
            if order_id:
                self.client.delete(f"/orderservice/api/order/{order_id}")

    @task
    def view_items(self):
        response = self.client.get("/inventoryservice/api/inventory")
        if response.status_code == 200 and response.json():
            item_id = response.json()[0].get("id")
            if item_id:
                self.client.get(f"/inventoryservice/api/inventory/{item_id}")

    @task
    def view_orders(self):
        response = self.client.get("/orderservice/api/order")
        if response.status_code == 200 and response.json():
            order_id = response.json()[0].get("id")
            if order_id:
                self.client.get(f"/orderservice/api/order/{order_id}")
