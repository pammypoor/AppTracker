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
            last: '',
            position: '',
            company: '',
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
                    <input className="profile-input" id="profile-position" type="text" value = {this.state.position} required placeholder="Position" onChange = {(e) => this.setState({position: e.target.value})}/>
                    <input className="profile-input" id="profile-company" type="text" value = {this.state.company} required placeholder="Company" onChange = {(e) => this.setState({company: e.target.value})}/>
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