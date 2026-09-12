
export const ConfirmButton = ({ btnText, onClick, className, btnType, form, disabled }) => {
  return (
        <>
            <button 
              type={btnType || "button"} 
              className={`btn btn-confirm manrope-semibold ${className || ""}`} 
              form={form}
              disabled={disabled}
              onClick={onClick}>
                {btnText || ""}
            </button>
        </>
  )
}
