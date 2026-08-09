import { ConfirmButton } from "../buttons/ConfirmButton"
import { useState } from "react"
import { useAuth } from "../contexts/AuthContext"
import { useNavigate } from "react-router-dom"

const LoginForm = () => {
  const {login, error } = useAuth();
  const navigate = useNavigate();

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();

      const success = await login(username, password);
      if(success) {
        navigate("/dashboard");
        return;
      }
  };

  return (
    <div className="card card-login">
      <h2 className="manrope-semibold text-center">Logga in</h2>
      <form onSubmit={handleSubmit}>
        <div className="input-group input-group-medium">
          <label htmlFor="username" className="manrope-semibold">Användarnamn</label>
          <input type="text" id="username" name="username" value={username} onChange={(e) => setUsername(e.target.value)} placeholder="Ange användarnamn" />
        </div>
        <div className="input-group input-group-medium">
          <label htmlFor="password" className="manrope-semibold">Lösenord</label>
          <input type="password" id="password" name="password" value={password} onChange={(e) => setPassword(e.target.value)} placeholder="Ange lösenord" />
        </div>
        {error && <p className="alert-danger">{error.message || error}</p>}
        <ConfirmButton btnType="submit" className="btn-login" btnText="Logga in" />
      </form>
      <div className="login-footer">
        <p>Har du inget konto?</p> 
        <a href="#">Registrera dig</a>
      </div>
    </div>
  )
}

export default LoginForm