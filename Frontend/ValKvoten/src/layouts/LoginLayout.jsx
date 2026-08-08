import LoginForm from '../components/forms/LoginForm';
const LoginLayout = () => {
  return (
    <div className="wrapper login-wrapper">
        <header>            
            <h1><a href="#" className="logo-text manrope-extra-bold">Val<span>Kvoten</span></a></h1>
        </header>

        <main>
            <LoginForm />
        </main>
    </div>
  )
}

export default LoginLayout