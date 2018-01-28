"use strict";

var loginVueInstance;

document.addEventListener("DOMContentLoaded", function (event) {
    Vue.use(VuelidateErrorExtractor.default, {
        template: VuelidateErrorExtractor.templates.foundation6
    });

    loginVueInstance = new Vue({
        el: '#loginForm',
        data: () => ({
            form: {
                email: null,
                password: null,
                rememberMe: false
            },
            userLoggedIn: false,
            sending: false
        }),

        validations: {
            form: {
                password: {
                    required: validators.required
                },
                email: {
                    required: validators.required
                }
            }
        },
        methods: {
            getValidationClass(fieldName) {
                const field = this.$v.form[fieldName]

                if (field) {
                    return {
                        'md-invalid': field.$invalid && field.$dirty
                    }
                }
            },
            login() {
                this.sending = true;
                var _this = this;

                var data = {
                    Email: this.form.email,
                    Password: this.form.password,
                    __RequestVerificationToken: document.getElementsByName("__RequestVerificationToken")[0].value
                };

                Vue.http.post('/Account/Login', data).then(response => {
                    //success callback
                    this.sending = false;
                    if (response.status === 200) {
                        document.location.href = "/";
                    } else {
                        _this.userLoggedIn = false;
                    }
                }, response => {
                    // error callback
                    _this.userLoggedIn = false;
                    this.sending = false;
                });
            },
            validateUser() {
                this.$v.$touch();

                if (!this.$v.$invalid) {
                    this.login();
                }
            }
        }

    });
});