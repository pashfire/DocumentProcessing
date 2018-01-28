"use strict";

document.addEventListener("DOMContentLoaded", function (event) {
    Vue.use(VuelidateErrorExtractor.default, {
        template: VuelidateErrorExtractor.templates.foundation6
    });

    var registrationVueInstance = new Vue({
        el: '#registrationForm',
        data: () => ({
            form: {
                firstName: null,
                lastName: null,
                role: null,
                email: null,
                password: null,
                confirmPassword: null
            },
            userSaved: false,
            sending: false,
            lastUser: null
        }),

        validations: {
            form: {
                firstName: {
                    required: validators.required,
                    minLength: validators.minLength(2)
                },
                lastName: {
                    required: validators.required,
                    minLength: validators.minLength(2)
                },
                password: {
                    required: validators.required,
                    minLength: validators.minLength(6)
                },
                confirmPassword: {
                    sameAsPassword: validators.sameAs('password')
                },
                role: {
                    required: validators.required
                },
                email: {
                    required: validators.required,
                    email: validators.email
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
            clearForm() {
                this.$v.$reset();
                this.form.firstName = null;
                this.form.lastName = null;
                this.form.password = null;
                this.form.confirmPassword = null;
                this.form.role = null;
                this.form.email = null;
            },
            saveUser() {
                this.sending = true;
                var _this = this;

                var data = {
                    FirstName: this.form.firstName,
                    LastName: this.form.lastName,
                    UserRole: this.form.role,
                    Email: this.form.email,
                    Password: this.form.password,
                    ConfirmPassword: this.form.confirmPassword,
                    __RequestVerificationToken: document.getElementsByName("__RequestVerificationToken")[0].value
                };

                Vue.http.post('/Account/Register', data).then(response => {
                    //success callback
                    _this.userSaved = true;
                    this.lastUser = `${this.form.firstName} ${this.form.lastName}`;
                    this.sending = false;
                    if (this.userSaved) {
                        document.location.href = "/";
                    }
                }, response => {
                    // error callback
                    _this.userSaved = false;
                    this.sending = false;
                });


            },
            validateUser() {
                this.$v.$touch();

                if (!this.$v.$invalid) {
                    this.saveUser();
                }
            }
        }

    });
});