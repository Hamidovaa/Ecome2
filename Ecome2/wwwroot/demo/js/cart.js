//function addTodo(value, price, imageUrl) {
//    // Retrieve existing data from localStorage
//    var existingData = localStorage.getItem('productObjects');
//    // Parse existing data into an array or initialize an empty array
//    var productObjects = existingData ? JSON.parse(existingData) : [];
//    var existingProduct = productObjects.find(product => product.ad === value);
//    if (existingProduct) {
//        existingProduct.quantity += 1;
//    } else {
//        var newProductObject = {
//            ad: value,
//            qiymet: price,
//            sekil: imageUrl,
//            quantity: 1
//        };
//        productObjects.push(newProductObject);
//    }

//    localStorage.setItem('productObjects', JSON.stringify(productObjects));
//    loadCartItems();
//}

//function addto_cart(element) {
//    var productItem = element.closest('.product-item'); // Find the closest product item
//    if (productItem) {
//        var productTitle = productItem.querySelector('.product-title a').textContent.trim(); // Get the product title
//        var productPrice = productItem.querySelector('.product-price span').textContent.trim().replace(' AZN', ''); // Get the product price and remove ' AZN'
//        var productImage = productItem.querySelector('.product-img img').getAttribute('src'); // Get the product image URL

//        alert(productTitle + " " + productPrice + " " + productImage); // Output for debugging

//        addTodo(productTitle, productPrice, productImage); // Add product to local storage
//    } else {
//        console.log("No .product-item element found within the clicked div.");
//    }
//}

//// Sepet içeriğini göstermek için bir fonksiyon oluşturun
//function showCartItems() {
//    var existingData = localStorage.getItem('productObjects');
//    var productObjects = existingData ? JSON.parse(existingData) : [];
//    var dropdownCartContainer = document.querySelector('.mini-products-list');

//    // Önce dropdown içeriğini temizleyin
//    dropdownCartContainer.innerHTML = '';

//    // Her ürün için bir döngü oluşturun
//    productObjects.forEach(function (product) {
//        var dropdownItem = document.createElement('ul');
//        dropdownItem.className = 'item-cart';
//        dropdownItem.innerHTML = `
//         <li class="item-cart">
//            <div class="product-img-wrap">
//                <a href="#"><img src="${product.sekil}" alt="" class="img-reponsive"></a>
//            </div>
//            <div class="product-details">
//                <div class="inner-left">
//                    <div class="product-name"><a href="#">${product.ad}</a></div>
//                    <div class="product-price">
//                         ${product.qiymet}<span>( x2)</span>
//                    </div>
//                </div>
//            </div>
//            <a href="#" onclick="removeCartItem()" class="e-del"><i class="ion-ios-close-empty"></i></a>
//        </li>
//        `;
//        dropdownCartContainer.appendChild(dropdownItem);
//    });
   
//    function removeCartItem(index) {
//        var existingData = localStorage.getItem('productObjects');
//        var productObjects = existingData ? JSON.parse(existingData) : [];
//        productObjects.splice(index, 1);
//        localStorage.setItem('productObjects', JSON.stringify(productObjects));
//        loadCartItems();
//    }
//}

//// Sayfa yüklendiğinde ve ürün eklendiğinde bu fonksiyonu çağırın
//showCartItems();

//// Ürün ekledikten sonra dropdown listesini güncelleyin
//function addTodo(value, price, imageUrl) {
//    var existingData = localStorage.getItem('productObjects');
//    var productObjects = existingData ? JSON.parse(existingData) : [];
//    var newProductObject = {
//        ad: value,
//        qiymet: price,
//        sekil: imageUrl
//    };
//    productObjects.push(newProductObject);
//    var updatedData = JSON.stringify(productObjects);
//    localStorage.setItem('productObjects', updatedData);

//    // Dropdown listesini güncelleyin
//    showCartItems();
//}



