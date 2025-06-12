# Migration Service Specifications

## Task Overview

You're asked by Happy Time Inc. to help them with the migration of products in their system.

## System Architecture

### Core Classes

#### Product

- Class that represents a product
- Methods:
  - `getName(): string` - retrieve product name (e.g., "puzzle_2_0", "ball_1", "ball_2")

#### Category

- Class that represents a product category
- Methods:
  - `getName(): string` - retrieve category name (e.g., "puzzle", "balls")

#### Group

- New class to represent groups of products and categories (e.g., "logic", "sport")
- Methods:
  - `getName(): string` - retrieve group name

#### PersistenceManager

- Interface/class representing the persistence layer
- Available methods:
  - `createCategory(name: string, g: Group): Category` - creates new category in given group
  - `createProduct(name: string, g: Group): Product` - creates new product in given group
  - `addProductToCategory(p: Product, c: Category): void` - adds product to category
  - `removeProductFromCategory(p: Product, c: Category): void` - removes product from category
  - `updateProduct(p: Product, newName: string): void` - updates product name
  - `updateCategory(c: Category, newName: string): void` - updates category name
  - `deleteProduct(p: Product): void` - removes product from application (by name, not reference)
  - `deleteCategory(c: Category): void` - removes category from application (by name, not reference)
  - `getCategories(p: Product): Category[]` - lists all categories for specified product
  - `getProducts(c: Category): Product[]` - lists all products in specified category

## Migration Types

The `MigrationType` enum defines four migration operations:

1. **COPY_SINGLE** - Copy single product/category (without dependencies)
2. **MOVE_SINGLE** - Move single product/category (without dependencies)
3. **COPY_MANY** - Copy product/category along with its dependencies
4. **MOVE_MANY** - Move product/category along with its dependencies

## Requirements

### Main Task

Implement the `MigrationService` class with the `migrate(object: Product | Category, group: Group, type: MigrationType)` method.

### Migration Behaviors

1. **Copy specified Product/Category to desired Group**
2. **Move specified Product/Category to desired Group**
3. **Copy (with dependencies) specified Product/Category to desired Group**
   - Example: Copying `puzzle_1` product from `puzzle` category should copy all three entities (puzzle_1, puzzle_2, puzzle category)
4. **Move (with dependencies) specified Product/Category to desired Group**
   - Example: Moving `puzzle_1` product from `puzzle` category should move all three entities

## Business Rules

### Naming Conventions

- Product and category names must be unique
- Attempting to create duplicates will throw an exception
- When creating copies, add "-copy" suffix to names

### Error Handling

- Service should be designed to preserve original data in case of persistence failures
- Use `console.log()` for debugging

### Technical Constraints

- Only use methods available in `PersistenceManager`
- Object constructors are not available directly

## Examples

### Copy with Dependencies

Copying `puzzle_1` product that belongs to `puzzle` category (which also contains `puzzle_2`) should copy all three entities to the target group.

### Move with Dependencies

Moving `puzzle_1` product that belongs to `puzzle` category (which also contains `puzzle_2`) should move all three entities to the target group.
