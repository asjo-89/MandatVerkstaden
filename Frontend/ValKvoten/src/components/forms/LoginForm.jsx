import { ConfirmButton } from "../buttons/ConfirmButton"

const LoginForm = () => {
  return (
    <div className="card card-login">
      <h2 className="manrope-semibold text-center">Logga in</h2>
      <form>
        <div className="input-group input-group-medium">
          <label htmlFor="email" className="manrope-semibold">E-post</label>
          <input type="email" id="email" name="email" placeholder="Ange e-postadress" />
        </div>
        <div className="input-group input-group-medium">
          <label htmlFor="password" className="manrope-semibold">Lösenord</label>
          <input type="password" id="password" name="password" placeholder="Ange lösenord" />
        </div>
        <ConfirmButton className="btn-login" btnText="Logga in" onClick={() => console.log("Logga in")} />
      </form>
      <div className="login-footer">
        <p>Har du inget konto?</p> 
        <a href="#">Registrera dig</a>
      </div>
    </div>
  )
}

export default LoginForm