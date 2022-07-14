import React from "react";
import axios from "axios";
import pbkdf2 from "pbkdf2/lib/sync";
import { FaEye, FaEyeSlash } from 'react-icons/fa';
import {Link} from "react-router-dom";

import "./SignInUpForm.css";

class SignInUpForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            email: '',
            password: '',
            token: sessionStorage.getItem('authorization'),
            errorMessage: '',
            showPassword: false
        }
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
        const renderSignInForm = (
            <div className="signin-form-container">
                <form className="signin-form">
                    <div className="signin-username-field">
                        <input className="signin-input" id="signin-email" type="text" value = {this.state.email} required placeholder="Email" onChange = {(e) => this.setState({email: e.target.value})}/>
                    </div>
                    <div className="signin-between-fields-spacing">
                        
                    </div>
                    <div className="signin-password-field">
                        <div className="signin-password-input-span">
                            <input className="signin-password-input" id="signin-password" type= {this.state.showPassword ? 'text': 'password'} value={this.state.password} required placeholder="Password" onChange={(e) =>this.setState({password: e.target.value})}></input>
                            <span className="signin-toggle-password" onClick={(e) => this.setState ({ showPassword: !this.state.showPassword})}>
                                {this.showPassword ? <FaEye/>: <FaEyeSlash/>}
                            </span> 
                        </div>
                        
                    </div>
                    <div className="signin-forgot-password">
                        <Link to="/forgotPassword">Forgot Password?</Link>
                    </div>
                    <div className="signin-button-field">
                        <button className="signin-button" onClick={this.onSubmitHandler}>Sign In</button>
                    </div>
                    {this.state.errorMessage && <div className="error-signin">{this.state.errorMessage}</div>}
                </form>
            </div>
        )


        return (
            <div className="signinup-form-container">
                {renderSignInForm}
            </div>
        )
    }
}

export default SignInUpForm;