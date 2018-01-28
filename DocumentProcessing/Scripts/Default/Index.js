"use strict";

var indexVueInstance;

document.addEventListener("DOMContentLoaded", function (event) {
    Vue.use(VueMaterial.default);
    Vue.use(vuelidate.default);

    appVueInstance = new Vue({
        el: '#indexPage',
        data: () => ({
        }),
        methods: {
            sendMessage() {
                window.alert('Send a message...')
            },
            doACall() {
                window.alert('Calling someone...')
            }
        }
    });
});