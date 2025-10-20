// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.ShootPatternMiniSuperShooter
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class ShootPatternMiniSuperShooter
  {
    private ShootPattern[] level1 = new ShootPattern[11]
    {
      new ShootPattern(0.2f, new bool[10]
      {
        true,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        true,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        true,
        false,
        false,
        false,
        false,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        false,
        true,
        false,
        false,
        false,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(2.2f, new bool[10], new int[10]
      {
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        1
      }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(2.2f, new bool[10], new int[10]
      {
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        1
      }),
      new ShootPattern(0.0f, new bool[10], new int[10]
      {
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        1
      })
    };
    private ShootPattern[] level2 = new ShootPattern[11]
    {
      new ShootPattern(0.2f, new bool[10]
      {
        true,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        true,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        true,
        false,
        false,
        false,
        false,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        false,
        true,
        false,
        false,
        false,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(1.7f, new bool[10], new int[10]
      {
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        1
      }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(1.7f, new bool[10], new int[10]
      {
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        1
      }),
      new ShootPattern(0.0f, new bool[10], new int[10]
      {
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        1
      })
    };
    private ShootPattern[] level3 = new ShootPattern[15]
    {
      new ShootPattern(0.2f, new bool[10]
      {
        true,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        true,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        true,
        false,
        false,
        false,
        false,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(1.7f, new bool[10]
      {
        false,
        false,
        false,
        true,
        false,
        false,
        false,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(1.7f, new bool[10]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        true,
        true,
        true,
        true,
        false,
        false,
        false,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        true,
        true,
        true,
        true,
        false,
        false,
        false,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(1.7f, new bool[10]
      {
        true,
        true,
        true,
        true,
        false,
        false,
        false,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        true,
        true,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        true,
        true,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(1.7f, new bool[10]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        true,
        true,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.0f, new bool[10], new int[10]
      {
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        1
      })
    };
    private ShootPattern[] level4 = new ShootPattern[11]
    {
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10], new int[10]
      {
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        1
      }),
      new ShootPattern(0.2f, new bool[10]
      {
        true,
        true,
        true,
        true,
        false,
        false,
        false,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        true,
        true,
        true,
        true,
        false,
        false,
        false,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(2.2f, new bool[10]
      {
        true,
        true,
        true,
        true,
        false,
        false,
        false,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        true,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10], new int[10]
      {
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        1
      }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        true,
        true,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.2f, new bool[10]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        true,
        true,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(2.2f, new bool[10]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        true,
        true,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.0f, new bool[10], new int[10]
      {
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        1
      })
    };
    private ShootPattern[] level5 = new ShootPattern[21]
    {
      new ShootPattern(2.1f, new bool[10], new int[10]
      {
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        1
      }),
      new ShootPattern(0.4f, new bool[10]
      {
        true,
        true,
        true,
        false,
        false,
        false,
        false,
        true,
        true,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.4f, new bool[10]
      {
        true,
        true,
        true,
        false,
        false,
        false,
        false,
        true,
        true,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.4f, new bool[10]
      {
        true,
        true,
        true,
        false,
        false,
        false,
        false,
        true,
        true,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.4f, new bool[10]
      {
        true,
        true,
        true,
        false,
        false,
        false,
        false,
        true,
        true,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(2.1f, new bool[10], new int[10]
      {
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        1
      }),
      new ShootPattern(0.4f, new bool[10]
      {
        true,
        true,
        true,
        false,
        false,
        false,
        false,
        false,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.4f, new bool[10]
      {
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.4f, new bool[10]
      {
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.4f, new bool[10]
      {
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.4f, new bool[10]
      {
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        false,
        false
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(2.1f, new bool[10], new int[10]
      {
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        1
      }),
      new ShootPattern(0.4f, new bool[10]
      {
        true,
        true,
        true,
        false,
        false,
        false,
        false,
        true,
        true,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.4f, new bool[10]
      {
        true,
        true,
        true,
        false,
        false,
        false,
        false,
        true,
        true,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.4f, new bool[10]
      {
        true,
        true,
        true,
        false,
        false,
        false,
        false,
        true,
        true,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.4f, new bool[10]
      {
        true,
        true,
        true,
        false,
        false,
        false,
        false,
        true,
        true,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(2.1f, new bool[10], new int[10]
      {
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        1
      }),
      new ShootPattern(0.4f, new bool[10]
      {
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        true,
        true,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.4f, new bool[10]
      {
        false,
        false,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.4f, new bool[10]
      {
        false,
        false,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 }),
      new ShootPattern(0.4f, new bool[10]
      {
        false,
        false,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true
      }, new int[10]{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 })
    };
    public ShootPattern[][] shootPatterns;
    private int _currentLevel;
    private int currentPattern;

    public int currentLevel => this._currentLevel;

    public bool isEmpty => this.currentPattern == this.shootPatterns[this.currentLevel].Length - 1;

    public ShootPatternMiniSuperShooter()
    {
      this.shootPatterns = new ShootPattern[5][]
      {
        this.level1,
        this.level2,
        this.level3,
        this.level4,
        this.level5
      };
    }

    public ShootPattern GetShootPattern(int level, int pattern)
    {
      return this.shootPatterns[level][pattern];
    }

    public ShootPattern GetNextShootPattern()
    {
      ShootPattern shootPattern = this.GetShootPattern(this.currentLevel, this.currentPattern);
      ++this.currentPattern;
      this.currentPattern = Mathf.Clamp(this.currentPattern, 0, this.shootPatterns[this.currentLevel].Length - 1);
      return shootPattern;
    }

    public ShootPattern GotoNextLevel()
    {
      ++this._currentLevel;
      this._currentLevel = Mathf.Clamp(this.currentLevel, 0, this.shootPatterns.Length - 1);
      this.currentPattern = 0;
      return this.GetShootPattern(this.currentLevel, this.currentPattern);
    }

    public ShootPattern SetLevel(int level)
    {
      this._currentLevel = level;
      this.currentPattern = 0;
      this._currentLevel = Mathf.Clamp(this.currentLevel, 0, this.shootPatterns.Length - 1);
      return this.GetShootPattern(this.currentLevel, this.currentPattern);
    }

    public void ResetPattern() => this.currentPattern = 0;

    public void Reset()
    {
      this._currentLevel = 0;
      this.currentPattern = 0;
    }
  }
}
