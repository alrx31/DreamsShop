# Use Cases

This document outlines the use cases for the DreamsShop application, categorized by service.

## Business Service

### Category

*   **Create**: Create a new category.
*   **Get**: Get a single category by its ID.
*   **GetAll**: Get all categories.
*   **Remove**: Remove a category.
*   **Update**: Update a category.

### DreamCategory

*   **Add**: Add a dream to a category.
*   **Delete**: Remove a dream from a category.

### Dreams

*   **Create**: Create a new dream.
*   **Delete**: Delete a dream.
*   **GetAll**: Get all dreams.
*   **GetCount**: Get the total number of dreams.
*   **GetOne**: Get a single dream by its ID.
*   **Update**: Update a dream.

### Order

*   **Create**: Create a new order.
*   **GetAllByUser**: Get all orders for a specific user.
*   **GetOne**: Get a single order by its ID.

## Identity Service

### ConsumerUser

*   **Delete**: Delete a consumer user.

### ConsumerUserAuth

*   **Login**: Log in as a consumer user.
*   **RefreshAccessToken**: Refresh the access token for a consumer user.
*   **Register**: Register a new consumer user.

### Producer

*   **Create**: Create a new producer.
*   **Delete**: Delete a producer.
*   **Get**: Get a producer by their ID.
*   **Update**: Update a producer's information.

### ProducerUserAuth

*   **Login**: Log in as a producer user.
*   **RefreshAccessToken**: Refresh the access token for a producer user.
*   **Register**: Register a new producer user.
