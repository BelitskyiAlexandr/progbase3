
using Terminal.Gui;

public class SignUpDialog : Dialog
{
    private UserRepository userRepository;
    public bool canceled;
    protected TextField loginInput;
    protected TextField passwordInput;
    protected TextField fullnameInput;

    public SignUpDialog(UserRepository userRepo)
    {
        this.userRepository = userRepo;
        this.Title = "Sign up";

        Button createRecord = new Button("Create");
        createRecord.Clicked += DialogConfirm;

        Button cancelRecord = new Button("Cancel");
        cancelRecord.Clicked += DialogCanceled;

        this.AddButton(createRecord);
        this.AddButton(cancelRecord);


        int rightColumnX = 20;

        Label fullname = new Label(2, 2, "Fullname ");
        fullnameInput = new TextField("")
        {
            X = rightColumnX,
            Y = Pos.Top(fullname),
            Width = 28,
        };
        this.Add(fullname, fullnameInput);

        Label loginLbl = new Label(2, 4, "Login ");
        loginInput = new TextField("")
        {
            X = rightColumnX,
            Y = Pos.Top(loginLbl),
            Width = 28,
        };
        this.Add(loginLbl, loginInput);

        Label passwordLbl = new Label(2, 6, "Password: ");
        passwordInput = new TextField("")
        {
            X = rightColumnX,
            Y = Pos.Top(passwordLbl),
            Width = 28,
        };
        this.Add(passwordLbl, passwordInput);

    }

    public User GetUser()
    {
        if (loginInput.Text.Length < 4)
        {
            MessageBox.ErrorQuery("Login error", "The login is too short", "Back");
            return null;
        }
        if (passwordInput.Text.Length < 4)
        {
            MessageBox.ErrorQuery("Password error", "The password is too short", "Back");
            return null;
        }
        User user = new User()
        {
            fullname = fullnameInput.Text.ToString(),
            username = loginInput.Text.ToString(),
            password = passwordInput.Text.ToString(),
        };
        if (userRepository.UserExistsByUsername(user.username))
        {
            MessageBox.ErrorQuery("SignUp error", "The login already exists", "Back");
            return null;
        }
        else
        {
            user = Hashing.SignUp(user.username, user.fullname, user.password, userRepository);
        }
        return user;
    }
    public void SetRepository(UserRepository repository)
    {
        this.userRepository = repository;
    }

    private void DialogCanceled()
    {
        this.canceled = true;
        Application.RequestStop();
    }

    private void DialogConfirm()
    {
        this.canceled = false;
        Application.RequestStop();
    }
}