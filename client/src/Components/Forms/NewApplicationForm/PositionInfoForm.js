function PositionInfoForm () {
    return (
        <div className="new-application-form-step1-container">
            <form className="new-application-form-step1" onSubmit = {this.onSubmitHandler}>
                <div className="new-application-company-field">
                    <input type="text" value = {this.state.company} required placeholder="Company" onChange = {this.inputCompanyHandler}/>
                </div>
                <div className="new-application-position-field">
                    <input type="text" value={this.state.position} required placeholder="Position" onChange={this.inputPositionHandler}/>
                </div>
                <div className="new-application-button-field">
                    <button className="new-application-button">Create</button>
                </div>
                {this.state.errorMessage && <div className="error-signin">{this.state.errorMessage}</div>}
            </form>
        </div>
    )
}

export default PositionInfoForm;