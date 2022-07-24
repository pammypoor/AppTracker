import React from "react";
import axios from "axios";

import "./ProfileForm.css";

class ProfileForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            email: '',
            username: '',
            first: '',
            last: ''
        }
    }

    hashValue = (value) => {
        var pbkdf2 = require('pbkdf2');
        const pbkdfKey = pbkdf2.pbkdf2Sync(value, '', 10000, 64, 'sha512');
        return pbkdfKey.toString('hex').toUpperCase();
    }

    onSubmitHandler = (e) => {
        e.preventDefault();
    }

    

    render() {
        const renderProfileForm = (
            <form className="profile-form">
                <div className="profile-2col-field">
                    <input className="profile-input" id="profile-email" type="text" value = {this.state.email} required placeholder="Email" onChange = {(e) => this.setState({email: e.target.value})}/>
                    <input className="profile-input" id="profile-username" type="text" value = {this.state.username} required placeholder="Username" onChange = {(e) => this.setState({username: e.target.value})}/>
                </div>
                <div className="profile-2col-field">
                    <input className="profile-input" id="profile-name" type="text" value = {this.state.name} required placeholder="First Name" onChange = {(e) => this.setState({first: e.target.value})}/>
                    <input className="profile-input" id="profile-last" type="text" value = {this.state.last} required placeholder="Last Name" onChange = {(e) => this.setState({last: e.target.value})}/>
                </div>
                <div className="profile-button-field">
                    <button className="profile-button" onClick={this.onSubmitHandler}>Update profile</button>
                </div>
            </form>
        )


        return (
            <div className="profile-form-container">
                {renderProfileForm}
            </div>
        )
    }
}

export default ProfileForm;