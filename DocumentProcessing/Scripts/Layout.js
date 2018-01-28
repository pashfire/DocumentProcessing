﻿"use strict";

var appVueInstance;

document.addEventListener("DOMContentLoaded", function (event) {
    Vue.use(VueMaterial.default);
    Vue.use(vuelidate.default);

    appVueInstance = new Vue({
        el: '#app',
        data: () => ({
            showNavigation: false,
            showSidepanel: false
        })
    });
});