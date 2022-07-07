import React from "react";
import axios from "axios";
import pbkdf2 from "pbkdf2/lib/sync";
import { FaUserTie } from 'react-icons/fa';

import "./SignInUpForm.css";

class SignInUpForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            email: '',
            password: '',
            token: sessionStorage.getItem('authorization'),
            errorMessage: ''
        }
    }

    inputEmailHandler = (e) => {
        this.setState({email: e.target.value});
    }

    inputPasswordHandler = (e) => {
        this.setState({password : e.target.value});
    }

    hashValue = (value) => {
        var pbkdf2 = require('pbkdf2');
        const pbkdfKey = pbkdf2.pbkdf2Sync(value, '', 10000, 64, 'sha512');
        return pbkdfKey.toString('hex').toUpperCase();
    }

    onSubmitHandler = (e) => {
        e.preventDefault();
        if(this.validateInput()){
            this.setState({errorMessage:''});
            axios.post('https://localhost:7199/Authentication/authenticate?email=' + this.state.email.toLowerCase() + '&password=' + this.hashValue(this.state.password))
            .then(response => {
                sessionStorage.setItem('authorization', response.data);
                window.location = '/Portal';
            })
            .catch(err => {
            })
        }

        this.setState( {email : ''});
        this.setState( {password: ''});
    }

    validateInput () {
        var regexEmail = new RegExp("^[a-zA-Z0-9.-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{3}$");
        var regexPassword = new RegExp("^[a-zA-Z0-9.,@!\s]+$");

        if(!regexEmail.test(this.state.email)){
            this.setState({errorMessage: 'Invalid email'});
            return false;
        }

        if(this.state.password.length < 8) {
            this.setState({errorMessage: 'Password must be at least 8 characters'});
            return false;
        } else {
            if(!regexPassword.test(this.state.password)) {
                this.setState( {errorMessage: 'Invalid passphrase'});
                return false;
            }
        }
        return true;     
    }

    render() {
        return (
            <div className="signin-form-container">
                <form className="signin-form" onSubmit = {this.onSubmitHandler}>
                    <div className="signin-username-field">
                        <input type="text" value = {this.state.email} required placeholder="Email" onChange = {this.inputEmailHandler}/>
                    </div>
                    <div className="signin-password-field">
                        <input type="password" value={this.state.password} required placeholder="Password" onChange={this.inputPasswordHandler}/>
                    </div>
                    <div className="signin-button-field">
                        <button className="signin-button">Sign In</button>
                    </div>
                    {this.state.errorMessage && <div className="error-signin">{this.state.errorMessage}</div>}
                </form>
            </div>
        )
    }
}

export default SignInUpForm;