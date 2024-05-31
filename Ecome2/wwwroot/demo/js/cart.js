










////function addTodo(value, price, imageUrl) {
////    // Retrieve existing data from localStorage
////    var existingData = localStorage.getItem('productObjects');
////    // Parse existing data into an array or initialize an empty array
////    var productObjects = existingData ? JSON.parse(existingData) : [];
////    var existingProduct = productObjects.find(product => product.ad === value);
////    if (existingProduct) {
////        existingProduct.quantity += 1;
////    } else {
////        var newProductObject = {
////            ad: value,
////            qiymet: price,
////            sekil: imageUrl,
////            quantity: 1
////        };
////        productObjects.push(newProductObject);
////    }

////    localStorage.setItem('productObjects', JSON.stringify(productObjects));
////    loadCartItems();
////}

////function addto_cart(element) {
////    var productItem = element.closest('.product-item'); // Find the closest product item
////    if (productItem) {
////        var productTitle = productItem.querySelector('.product-title a').textContent.trim(); // Get the product title
////        var productPrice = productItem.querySelector('.product-price span').textContent.trim().replace(' AZN', ''); // Get the product price and remove ' AZN'
////        var productImage = productItem.querySelector('.product-img img').getAttribute('src'); // Get the product image URL

////        alert(productTitle + " " + productPrice + " " + productImage); // Output for debugging

////        addTodo(productTitle, productPrice, productImage); // Add product to local storage
////    } else {
////        console.log("No .product-item element found within the clicked div.");
////    }
////}

////// Sepet içeriğini göstermek için bir fonksiyon oluşturun
////function showCartItems() {
////    var existingData = localStorage.getItem('productObjects');
////    var productObjects = existingData ? JSON.parse(existingData) : [];
////    var dropdownCartContainer = document.querySelector('.mini-products-list');

////    // Önce dropdown içeriğini temizleyin
////    dropdownCartContainer.innerHTML = '';

////    // Her ürün için bir döngü oluşturun
////    productObjects.forEach(function (product) {
////        var dropdownItem = document.createElement('ul');
////        dropdownItem.className = 'item-cart';
////        dropdownItem.innerHTML = `
////         <li class="item-cart">
////            <div class="product-img-wrap">
////                <a href="#"><img src="${product.sekil}" alt="" class="img-reponsive"></a>
////            </div>
////            <div class="product-details">
////                <div class="inner-left">
////                    <div class="product-name"><a href="#">${product.ad}</a></div>
////                    <div class="product-price">
////                         ${product.qiymet}<span>( x2)</span>
////                    </div>
////                </div>
////            </div>
////            <a href="#" onclick="removeCartItem()" class="e-del"><i class="ion-ios-close-empty"></i></a>
////        </li>
////        `;
////        dropdownCartContainer.appendChild(dropdownItem);
////    });

////    function removeCartItem(index) {
////        var existingData = localStorage.getItem('productObjects');
////        var productObjects = existingData ? JSON.parse(existingData) : [];
////        productObjects.splice(index, 1);
////        localStorage.setItem('productObjects', JSON.stringify(productObjects));
////        loadCartItems();
////    }
////}

////// Sayfa yüklendiğinde ve ürün eklendiğinde bu fonksiyonu çağırın
////showCartItems();

////// Ürün ekledikten sonra dropdown listesini güncelleyin
////function addTodo(value, price, imageUrl) {
////    var existingData = localStorage.getItem('productObjects');
////    var productObjects = existingData ? JSON.parse(existingData) : [];
////    var newProductObject = {
////        ad: value,
////        qiymet: price,
////        sekil: imageUrl
////    };
////    productObjects.push(newProductObject);
////    var updatedData = JSON.stringify(productObjects);
////    localStorage.setItem('productObjects', updatedData);

////    // Dropdown listesini güncelleyin
////    showCartItems();
////}


//----------------------------------------------------------
//    // Sepet verilerini kaydetmek
//    function saveCartToLocalStorage(cart) {
//        localStorage.setItem('cart', JSON.stringify(cart));
//        console.log('Cart saved to localStorage:', cart);
//        renderCartItems(); // Sepet öğelerini güncelle
//    }

//// Sepet verilerini okumak
//function loadCartFromLocalStorage() {
//    const cart = JSON.parse(localStorage.getItem('cart')) || [];
//    console.log('Cart loaded from localStorage:', cart);
//    return cart;
//}

//// Sepet verilerini HTML'de göstermek
//function renderCartItems() {
//    const cart = loadCartFromLocalStorage();
//    const cartItemsContainer = document.getElementById('cartItems');
//    cartItemsContainer.innerHTML = ''; // Önceki içeriği temizle

//    cart.forEach(item => {
//        const listItem = document.createElement('tr');
//        listItem.classList.add('item_cart');
//        listItem.innerHTML = `
//            <td class="product-name flex align-center">
//                <a href="#" class="remove-cart x-remove pr-3" data-product-id="${item.productId}"><i class="ion-ios-close-empty">X</i></a>
//                <div class="product-img">
//                    <img width="100px" height="100px" src="~/UploadProducts/${item.imageUrlBase}" alt="${item.productName}">
//                </div>
//                <div class="product-info">
//                    <a href="#" title="">${item.productName}</a>
//                </div>
//            </td>
//            <td class="bcart-quantity single-product-detail">
//                <div class="single-product-info">
//                    <div class="e-quantity">
//                        <input type="number" step="1" min="1" max="999" name="quantity" value="${item.quantity}" class="qty input-text js-number" size="4" data-product-id="${item.productId}">
//                        <div class="tc pa">
//                            <a class="js-plus quantity-right-plus" data-product-id="${item.productId}"><i class="fa fa-caret-up"></i></a>
//                            <a class="js-minus quantity-left-minus" data-product-id="${item.productId}"><i class="fa fa-caret-down"></i></a>
//                        </div>
//                    </div>
//                </div>
//            </td>
//            <td class="total-price">
//                <h4>Total</h4>
//                <p class="price">${item.total.toFixed(2)}</p>
//            </td>
//        `;
//        cartItemsContainer.appendChild(listItem);
//    });

//    updateTotalPrices();
//}

//// Fonksiyonlarla sepeti güncelle
//document.addEventListener('DOMContentLoaded', function () {
//    renderCartItems();

//    const form = document.querySelector('form[action="/Cart/AddToCartt"]');
//    form.addEventListener('submit', function (event) {
//        event.preventDefault();

//        const productId = document.querySelector('input[name="productId"]').value;
//        const selectedColorId = document.querySelector('select[name="SelectedColorId"]').value;
//        const selectedColorName = document.querySelector('select[name="SelectedColorId"] option:checked').text;
//        const selectedSizeId = document.querySelector('select[name="SelectedSizeId"]').value;
//        const selectedSizeName = document.querySelector('select[name="SelectedSizeId"] option:checked').text;
//        const quantity = document.querySelector('input[name="Quantity"]').value;

//        console.log('Form data:', { productId, selectedColorId, selectedColorName, selectedSizeId, selectedSizeName, quantity });

//        let cart = loadCartFromLocalStorage();

//        let cartItem = cart.find(item => item.productId === parseInt(productId) && item.color === selectedColorName && item.size === selectedSizeName);
//        if (cartItem) {
//            cartItem.quantity += parseInt(quantity);
//        } else {
//            cart.push({
//                productId: parseInt(productId),
//                productName: '@Model.products.FirstOrDefault().Title',
//                price: '@Model.products.FirstOrDefault().Price',
//                quantity: parseInt(quantity),
//                imageUrlBase: '@Model.products.FirstOrDefault().ImgUrlBase',
//                color: selectedColorName,
//                size: selectedSizeName,
//                total: parseInt(quantity) * parseFloat('@Model.products.FirstOrDefault().Price')
//            });
//        }

//        saveCartToLocalStorage(cart);
//        alert('Product added to cart!');
//    });

//    // Update total prices
//    function updateTotalPrices() {
//        const cart = loadCartFromLocalStorage();
//        const grandTotal = cart.reduce((total, item) => total + item.total, 0);

//        document.querySelector('.cart-subtotal td').textContent = grandTotal.toFixed(2);
//        document.querySelector('.order-total td').textContent = grandTotal.toFixed(2);
//    }

//    // Increment quantity
//    document.querySelectorAll('.js-plus').forEach(button => {
//        button.addEventListener('click', function () {
//            const productId = this.getAttribute('data-product-id');
//            const input = document.querySelector(`input[name="quantity"][data-product-id="${productId}"]`);
//            let currentValue = parseInt(input.value);
//            input.value = currentValue + 1;

//            updateCartItem(productId, input.value);
//        });
//    });

//    // Decrement quantity
//    document.querySelectorAll('.js-minus').forEach(button => {
//        button.addEventListener('click', function () {
//            const productId = this.getAttribute('data-product-id');
//            const input = document.querySelector(`input[name="quantity"][data-product-id="${productId}"]`);
//            let currentValue = parseInt(input.value);
//            if (currentValue > 1) {
//                input.value = currentValue - 1;

//                updateCartItem(productId, input.value);
//            }
//        });
//    });

//    // Remove item from cart
//    document.querySelectorAll('.remove-cart').forEach(button => {
//        button.addEventListener('click', function (e) {
//            e.preventDefault();
//            const productId = this.getAttribute('data-product-id');
//            removeCartItem(productId);
//        });
//    });

//    // Update cart item quantity
//    function updateCartItem(productId, quantity) {
//        let cart = loadCartFromLocalStorage();
//        let cartItem = cart.find(item => item.productId === parseInt(productId));
//        if (cartItem) {
//            cartItem.quantity = parseInt(quantity);
//            cartItem.total = cartItem.quantity * cartItem.price;
//        }
//        saveCartToLocalStorage(cart);
//        updateTotalPrices();
//        updateOneProductTotalPrices(productId, cartItem.total);
//    }

//    // Remove cart item
//    function removeCartItem(productId) {
//        let cart = loadCartFromLocalStorage();
//        cart = cart.filter(item => item.productId !== parseInt(productId));
//        saveCartToLocalStorage(cart);

//        const productRow = document.querySelector(`.remove-cart[data-product-id="${productId}"]`).closest('.item_cart');
//        productRow.remove();
//        updateTotalPrices();
//    }

//    // Apply coupon
//    document.getElementById('applyCouponBtn').addEventListener('click', function (e) {
//        e.preventDefault();
//        const couponCode = document.getElementById('couponCode').value;
//        applyCoupon(couponCode);
//    });

//    function applyCoupon(code) {
//        $.ajax({
//            url: '/Checkout/ApplyCoupon',
//            method: 'POST',
//            data: { code: code },
//            success: function (response) {
//                updateCartTotal(response.grandTotal);
//                console.log('Coupon applied successfully');
//            },
//            error: function () {
//                console.error('Error applying coupon');
//            }
//        });
//    }

//    function updateCartTotal(grandTotal) {
//        document.querySelector('.cart-subtotal td').textContent = grandTotal.toFixed(2);
//        document.querySelector('.order-total td').textContent = grandTotal.toFixed(2);
//    }

//    function updateOneProductTotalPrices(productId, itemTotal) {
//        const productRow = document.querySelector(`.remove-cart[data-product-id="${productId}"]`).closest('.item_cart');
//        productRow.querySelector('.total-price p').textContent = itemTotal.toFixed(2);
//    }
//});


