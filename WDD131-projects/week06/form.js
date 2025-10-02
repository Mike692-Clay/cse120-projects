```javascript
// form.js

// Product data array
const products = [
  {
    id: "fc-1888",
    name: "flux capacitor",
    averagerating: 4.5
  },
  {
    id: "fc-2050",
    name: "power laces",
    averagerating: 4.7
  },
  {
    id: "fs-1987",
    name: "time circuits",
    averagerating: 3.5
  },
  {
    id: "ac-2000",
    name: "low voltage reactor",
    averagerating: 3.9
  },
  {
    id: "jj-1969",
    name: "warp equalizer",
    averagerating: 5.0
  }
];

// Populate the product select dropdown
function populateProducts() {
  const selectElement = document.getElementById("productName");

  // Clear any existing options (except placeholder)
  // Keep the first option (placeholder) intact
  selectElement.length = 1;

  // Create an option for each product in the array
  products.forEach(product => {
    const option = document.createElement("option");
    option.value = product.id; // value is product id
    option.textContent = product.name; // display name
    selectElement.appendChild(option);
  });
}

// Call the function after DOM is loaded
document.addEventListener("DOMContentLoaded", populateProducts);
```
