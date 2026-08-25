
export const ConfirmButton = ({ btnText, onClick, className, btnType, form }) => {
  return (
        <>
            <button 
              type={btnType || "button"} 
              className={`btn btn-confirm manrope-semibold ${className || ""}`} 
              form={form}
              onClick={onClick}>
                {btnText || ""}
            </button>
        </>
  )
}
